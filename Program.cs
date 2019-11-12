using System;
using System.Threading;
using twilifi.unifi;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace twilifi
{
    class Program
    {
        public static void Main(string[] args)
        {
            var cameras = new UnifiCameras();

            var accountSid = System.Environment.GetEnvironmentVariable("TWILIO_SID");
            var authToken = System.Environment.GetEnvironmentVariable("TWILIO_TOKEN");

            var twilioNumber = System.Environment.GetEnvironmentVariable("TWILIO_NUMBER");
            var sendNumber = System.Environment.GetEnvironmentVariable("SEND_NUMBER");

            TwilioClient.Init(accountSid, authToken);

            var startTime = DateTimeOffset.Now.AddMinutes(-2).ToUnixTimeMilliseconds();

            while (true)
            {
                var endTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                // get motion events from camera
                var events = cameras.GetEvents(
                    "motion",
                    startTime,
                    endTime
                ).GetAwaiter().GetResult();

                Console.WriteLine($"got events for {startTime} to {endTime}");
                // Reset base
                startTime = endTime;

                if (events.Count > 0)
                {
                    var message = MessageResource.Create(
                        body: $"Noticed {events.Count} motion events on camera system.",
                        from: new Twilio.Types.PhoneNumber(twilioNumber),
                        to: new Twilio.Types.PhoneNumber(sendNumber)
                    );

                    Console.WriteLine(message.Sid);
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }
    }
}

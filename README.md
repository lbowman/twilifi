# Overview

Small program to experiment with the [Twilio](https://www.twilio.com/) api.
I have never used the notifications of my camera system at home. In order to
play with the Twilio api I decided to see if I could get events when the cameras
detect motion and then send a notification. The `motion` event on the cameras is
also terrible(many false positives) so it may be an opportunity to apply image 
classification beyond what the platform provides.

## Current State

The dotnet core application runs in a 1 minute loop and looks for new motion events
that the cameras detected. If there are any new ones it will send a text message that
only contains the count. 

## Possible Enhancements

* Use camera api to get thumbnails and send them along in an mms
* Provide the ability to restrict which cameras are included
* Fancy ML to ignore things like delivery drivers

## Configuration

All configuration is done via environment variables

| Name | Description |
|------|-------------|
|UNIFI_USER| username that is used to log into the unifi protect controller|
|UNIFI_PASS| password for the unifi protect controller|
|UNIFI_ENDPOINT| Url for the unifi controller|
|TWILIO_SID| Twilio sid|
|TWILIO_TOKEN| Twilio token|
|TWILIO_NUMBER| Twilio phone number with leading `+`|
|SEND_NUMBER| Phone number to send sms to with leading `+`|

## Unifi Api

This doesn't really exist, but there are some [links on the internet](https://community.ui.com/questions/Unifi-Protect-API-or-Motion-alerts-hit-API/b5c6cf44-651c-4751-98a8-c55d7ce9bd5f?page=2
)
that will help figure out how to extract data.

```
curl -k https://192.168.1.30/api/ump/info

# Get a bearer token with username/pwd
curl -k -X POST -H "Content-Type: application/json" -d '{"username": "...", "password": "..."}' https://192.168.1.30:7443/api/auth

token="..."

# Get an access key with bearer token - not sure what this is used for
curl -k -X POST -H "Authorization: Bearer $token" https://192.168.1.30:7443/api/auth/access-key

# Get recent events - would like to filter for cameras
curl -k -H "Authorization: Bearer $token" https://192.168.1.30:7443/api/events?end=1573510042317&start=1573423642317&type=motion

https://192.168.1.30:7443/api/thumbnails/5dc98e3902353b03e70010aa?accessKey=...&h=79&w=140
```

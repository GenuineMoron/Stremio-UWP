# Stremio
A Universal Windows Program (UWP) WebView app for Windows and Xbox.

![Banner](https://github.com/Misunderstood-Wookiee/Stremio-UWP/assets/22002023/a7291857-0935-4067-a2db-03fb7adaae33)

## Streaming Torrents (Recommended Setup)

The app loads the Stremio web UI, which cannot run a local torrent engine on Xbox. The recommended way to stream torrents is through a **debrid service** — these cache torrents on their servers and return a direct HTTPS link, which plays without any local server.

**Setup (one-time, inside the Stremio app):**

1. Sign up for a debrid service. [Real-Debrid](https://real-debrid.com) (~€3/month) and [TorBox](https://torbox.app) (has a free tier) are popular options.
2. Get your API key from the debrid provider's website.
3. Open the [Torrentio addon configurator](https://torrentio.strem.fun), select your provider, paste your API key, and copy the generated addon URL.
4. In the Stremio app → **Addons** → paste the URL and install.

Most popular torrents are already cached and stream instantly at full speed. No local server or extra hardware needed.

## Connecting a Streaming Server (Advanced)

For full torrent support without a debrid service, run [stremio-service](https://github.com/Stremio/stremio-service) or the [community Docker image](https://github.com/tsaridas/stremio-docker) on a PC on your network, then set the server URL in **Settings → Streaming** inside the app.

The server must be reachable over HTTPS. The Docker image supports `IPADDRESS=<your-LAN-IP>` to auto-generate a valid certificate.

## Known Limitations

- Streaming services that require Widevine DRM (Netflix, Disney+, etc.) will not work — Xbox only supports PlayReady DRM, and obtaining PlayReady certification requires Microsoft approval.
- A WebView2 runtime must be available on the device. Xbox support for WebView2 is currently unconfirmed; sideloading in Dev Mode is the recommended path for Xbox.

## Legal

- [GPLv3](https://choosealicense.com/licenses/gpl-3.0/)
- This project is not affiliated with www.stremio.com

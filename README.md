# Stremio for Xbox

Watch movies and TV shows on your Xbox One or Xbox Series X/S using the Stremio web app.

![Banner](https://github.com/Misunderstood-Wookiee/Stremio-UWP/assets/22002023/a7291857-0935-4067-a2db-03fb7adaae33)

---

## What you need

- An Xbox One, Xbox One S, or Xbox One X
- A phone, tablet, or PC to use alongside your Xbox for a few steps
- A free [Stremio account](https://www.stremio.com) (sign up on your phone/PC first)

---

## Step 1 — Put your Xbox into Developer Mode

Developer Mode lets you install apps that aren't on the Microsoft Store. You only do this once.

1. On your Xbox, open the **Microsoft Store** and search for **"Dev Mode Activation"**.
2. Download and open the **Xbox Dev Mode Activation** app — it's free.
3. Follow the on-screen steps. At one point it will show you a code and a website address.
4. On your phone or PC, go to that website and enter the code. You'll need to pay a **one-time $19 USD fee** to Microsoft to register as a developer.
5. Once approved, go back to the Xbox app and press **Switch and Restart**. Your Xbox will reboot into Developer Mode.

> **Note:** You can switch back to Retail Mode at any time from the Dev Home screen. Your games and saves are untouched.

---

## Step 2 — Find your Xbox's IP address

You need this to send the app to your Xbox from your PC.

1. In Developer Mode, open the **Dev Home** app (it launches automatically after restart).
2. Look for your **IP address** on the screen — it looks something like `192.168.1.50`.
3. Write it down.

---

## Step 3 — Download the Stremio app file onto your PC

1. On your PC, go to the [Releases page](../../releases) of this project.
2. Download the latest `.appx` file (or `.zip` — extract it if so).

---

## Step 4 — Install the app on your Xbox

1. On your PC, open a browser and go to:
   ```
   http://YOUR-XBOX-IP:11443
   ```
   Replace `YOUR-XBOX-IP` with the address from Step 2.
   Example: `http://192.168.1.50:11443`

2. Your browser may warn you the connection isn't secure — click **Advanced** and proceed anyway. This is normal for the Xbox device portal.

3. You'll see the **Xbox Device Portal**. Click **My Games & Apps** in the left menu.

4. Click **Add** (or "Install") and select the `.appx` file you downloaded.

5. Wait for it to upload and install — takes about 30 seconds.

6. When it says "Installation successful", the app is on your Xbox.

---

## Step 5 — Switch back to Retail Mode and launch Stremio

1. On your Xbox in Dev Home, go to **Return to Retail Mode** and confirm.
2. Your Xbox restarts back to the normal home screen.
3. Go to **My Games & Apps → Apps** and you'll see **Stremio** listed there.
4. Launch it and sign in with your Stremio account.

---

## Step 6 — Set up streaming (so you can actually watch things)

The app shows the Stremio library, but to stream content you need to connect a source. The easiest way is a **debrid service** — it grabs torrents for you and sends a direct video link to your Xbox. No extra hardware needed.

### Option A — Debrid service (easiest, small monthly cost)

A debrid service costs around **€3–5/month** and streams instantly with no buffering.

**Popular choices:**
- [Real-Debrid](https://real-debrid.com) — ~€3/month, most popular
- [TorBox](https://torbox.app) — has a free tier

**Setup (do this on your phone or PC):**

1. Sign up for a debrid service and get your **API key** from their website (usually under Account or Settings).
2. Go to [torrentio.strem.fun](https://torrentio.strem.fun) in your browser.
3. Pick your debrid provider, paste in your API key, then click **Install** or copy the addon URL.
4. In the Stremio app on your Xbox, go to **Addons**, paste the URL, and install it.

Done. Search for any movie or show and you'll see streaming links appear.

### Option B — Home server (free, needs a PC on your network)

If you have a PC running on your home network, you can run Stremio's streaming server on it.

1. On your PC, download and install [Stremio Service](https://github.com/Stremio/stremio-service).
2. Note your PC's local IP address (e.g. `192.168.1.10`).
3. In the Stremio app on your Xbox, go to **Settings → Streaming** and enter your server's address.

---

## Controls

| Button | Action |
|--------|--------|
| D-pad / Left stick | Navigate menus |
| A | Select / confirm |
| B | Go back |
| Menu button | Open options |

---

## Known limitations

- **Netflix, Disney+, and other DRM-protected services won't work.** Xbox only supports PlayReady DRM, and this app cannot get PlayReady certification. Use those apps' official Xbox Store versions instead.
- This app is not affiliated with Stremio.

---

## Legal

Licensed under [GPLv3](https://choosealicense.com/licenses/gpl-3.0/). Not affiliated with stremio.com.

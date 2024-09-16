# ReiTunes

My personal music library system. It's gone through a few iterations.

![Dark UI](https://res.cloudinary.com/reilly-wood/image/upload/v1608417001/reitunes/dark.jpg)
![Light UI](https://res.cloudinary.com/reilly-wood/image/upload/v1608417001/reitunes/light.jpg?foo=bar)

## Why?

When I started, I had 3 priorities for my music collection:

1. My collection needs to last forever (or at least until I kick the bucket) 
2. It should integrate well with music podcasts and online series
3. It should have a good native Windows app that can sync across multiple devices and operate offline (this is less of a priority these days)

Those weren't really met by any existing music services. I don't know if any given streaming service will be around next year, let alone 40 years from now. iTunes Match is OK as a service but the Windows client is not.

## What?

These days I spend most of my time working on a Rust+web UI version of ReiTunes (in `reitunes-rs`).

The Windows client is a UWP application. It's quite polished but probably won't receive much investment going forward because I'm on Windows less these days.

Once upon a time I also had a Blazor client but that's been retired.

## Architecture

I spent a lot of effort making ReiTunes work well offline and with multiple clients.

On the server side, a web API acts as a central sync point for library metadata. Music files are stored in cloud object storage and accessible over HTTPS.

Library metadata changes are treated as events; they are serialized to JSON and stored in SQLite locally immediately, then pushed to the server asynchronously later. You can think of the events like Git commits.

The entire library is rebuilt from events on launch (more than fast enough; sub-10ms in the Rust version).

## Acknowledgments

The library synchronization approach is heavily influenced by [Building offline-first web and mobile apps using event-sourcing](https://flpvsk.com/blog/2019-07-20-offline-first-apps-event-sourcing/) by Andrey Salomatin.

ReiTunes was partially motivated by [Tom MacWright's excellent post about his own music library](https://macwright.com/2020/01/27/my-music-library.html).

## Disclaimers

This is provided with no promises around support or features. You will need to run your own server and bring your own music. Happy to chat about feature ideas, but I am writing this for myself, for fun.

Music piracy is bad; you should buy your music from a reputable source like [Bandcamp](https://bandcamp.com/).

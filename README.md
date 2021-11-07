# Hack2021 - Hey Spotify

This is the repo for Hey Spotify. Unfortunately, our naming convention project side was not the best, so here's what everything is.

# TestSpotify - Solution
This is our overall solution that contains all of our code. Open it up to see all of the projects and swap between which ones to run.

# TestMic - Project
This is our library we wrote to allow for interfacing with AssemblyAI. It hides all the code nicely for converting speech to text.

# TestSpotify - Project
This is our library we wrote for interfacing with Spotify. We make calls to Spotify using REST API (we used SpotifyAPI-NET for this since it had already created all of the classes we would need), and we set up some code to allow for intelligent parsing of text to commands.

# AccessibleSpotify - Project
This is our actual FrontEnd written in C# and WPF. Running this project allows you to fully use HeySpotify.

# Before you can use...
You'll first need to update the Spotify Auth key located in "SpotifyCommands.cs" to your own Auth Key so you can run it. You'll also want to update the chosen mic in the TestMic project to use whatever mic you want (it is an index, and in our case we set it to 2).
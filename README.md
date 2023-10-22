# Jellyfin Extended Datetime parser for Daily Episodes.

This plugin support the standard Japanese media date format for daily shows episodes. This plugin only concerns itself with episodes,
If you want metadata for the series itself or the season you should use `NFO` files.

## Expected filename formats.

Date can be in the following format (YYYY-mm-dd `2023-09-16` - date separator can be `-`, `.` or `_`) or (YY?YYmmdd `20230916 or 230916`) (dates before 2000 should be in YYYYmmdd format),as we prefix two digits to the year to make it a four digit year with `20`.

So the filename itself can contain any of the previous date formats. However, the place where the date is located is important and must comply with the following spec. `{date}` refers to the date in any of the previous formats.

* `{date}` title [720p].mkv
* Series Title - `{date}` - title [720p].mkv
* Title `{date}`.mkv

## Special Format
This format is specially added to support a better looking title.
* `{date}` Series title -? `(#0|ep0|DVD1|DVD1.1|SP1|SP1.1)` -? title [720p].mkv

The generated title will be `(#0|ep0|DVD1|DVD1.1|SP1|SP1.1) - title`.

## Installing the plugin.

Go to the Releases page and download the latest release.

create a folder named `DAILYExtender` in the `plugins` directory inside your Jellyfin data directory. You can find your directory by going to Dashboard, and noticing the Paths section. Mine is the root folder of the default Metadata directory.

Unzip the downloaded file and place the resulting files in the `plugins/DAILYExtender` restart jellyfin.
Go to your Japanese library Make sure `DAILYExtender` is on the top of your `Metadata readers` list. Disable all external metadata sources.
And Only enable `Image fetchers (Episodes):` - `Screen grabber (FFmpeg)`. if you don't have a local image for the episode, it will be fetched from the video file itself.

## Build and Installing from source

1. Clone or download this repository.
1. Ensure you have .NET Core SDK setup and installed.
1. Build plugin with following command.
    ```
    dotnet publish --configuration Release --output bin
    ```
1. Create folder named `DAILYExtender` in the `plugins` directory inside your Jellyfin data
   directory. You can find your directory by going to Dashboard, and noticing the Paths section.
   Mine is the root folder of the default Metadata directory.
    ```
    # mkdir <Jellyfin Data Directory>/plugins/DAILYExtender/
    ```
1. Place the resulting files from step 3 in the `plugins/DAILYExtender` folder created in step 4.
    ```
    # cp -r bin/*.dll <Jellyfin Data Directory>/plugins/DAILYExtender/`
    ```
1. Be sure that the plugin files are owned by your `jellyfin` user:
    ```
    # chown -R jellyfin:jellyfin /var/lib/jellyfin/plugins/DAILYExtender/
    ```
1. If performed correctly you will see a plugin named DAILYExtender in `Admin -> Dashboard -> Advanced -> Plugins`.

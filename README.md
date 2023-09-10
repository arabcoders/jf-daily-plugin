# Jellyfin Extended Datetime parser for Episodes.

## Usage

Filenames can be in the following format
* YYYMMDD series title (- title)?
* YYMMDD series title (- title)?
* series title = (\[|\(])YYYY-MM-DD(\)\]) (- title)?

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

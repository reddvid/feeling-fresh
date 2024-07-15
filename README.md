# Fresh

A set of tools/mini-programs to ease the process of reinstalling Windows.

# Be Fresh

Moniker of before fresh 😉, a set of tools to backup: 

* Start menu layout
* Minecraft worlds/installations
* (WIP) Elgato Stream Deck profiles
* (WIP) OBS Studio Scenes/Profiles

# Feeling Fresh (FF)

A personal Windows app built with WPF to fast-forward (FF) app installation after a fresh installation of Windows OS.

Most app installers like Ninite or PatchMyPC can do better but their app list does not do it for me.

This app uses SQL commands to fetch, add, update, and delete objects from a cloud database.

(WIP) Add a way to install apps through <code>winget</code>.

(WIP) Add a way to reinstall Stream Deck plugins.

## Notes
* CRUD operations are inside a DBHelper class (gitignored).
* Double-clicking App item will launch DuckDuckGo's I'm feeling lucky URL that will direct you (mostly) to the download page in a browser.
* .exe installers are still downloaded on the machine and requires manual running
* Requires prior database setup:
  * Database name: AppsList
  * Columns:
  
  | ID (PK) | AppName |
  |---------|---------|
  |    0    | VS Code |
  |         |         |
* DBHelper.cs boilerplate is inside assets/Helpers/


## Screenshots
![Main Window](/assets/main-window.png)

![Sort List](/assets/sorted-list.png)

![Edit App](/assets/edit-app-item.png)

## Thanks to:
[Kinnara/ModernWPF](https://github.com/Kinnara/ModernWpf)
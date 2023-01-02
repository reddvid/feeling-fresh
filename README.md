# Feeling Fresh (FF)

A personal Windows app built with WPF to fast-forward (FF) app installation after a fresh install of Windows OS.

Most of app installers Ninite, and PatchMyPC can do better but their app list does not do it for me.

This app uses SQL commands to fetching, add, update, and delete objects from a cloud database.

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



## Screenshots
![Main Window](/assets/main-window.png)

![Sort List](/assets/sorted-list.png)

![Edit App](/assets/edit-app-item.png)

## Thanks to:
[Kinnara/ModernWPF](https://github.com/Kinnara/ModernWpf)
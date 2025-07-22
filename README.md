# ClientToDatabaseNameUpdater

A simple Windows Forms tool to batch update names from EO client files into MySQL database tables.

## Features

- **NPC Name Updater:**  
  Update NPC names in `cq_npc` and `cq_dynanpc` tables using names from `npc.ini`.

- **Monster Name Updater:**  
  Update monster names in `cq_monstertype` table using names from `monster.fdb`.

- **Item Name Updater:**  
  Update item names in `cq_itemtype` table using names from `itemtype.fdb`.

- **Magic Name Updater:**  
  Update magic names in `cq_magictype` table using names from `magictype.fdb`.

- **Map Name & Description Updater:**  
  Update map names and description in `cq_map` table using data from `gamemap.ini`.

- **Single Click Database Connection:**  
  Easily connect/disconnect to MySQL 8.x with default credentials (configurable).

- **EO Client Folder Selection:**  
  Quickly set your EO client folder with a folder browser.

- **Auto Field Mapping:**  
  Handles field name mapping and trims field length to match DB schema.

- **Full Source Included:**  
  No external DLL needed (FDB loader code included).

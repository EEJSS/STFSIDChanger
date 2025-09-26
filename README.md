# STFSIDChanger

A lightweight tool for batch-editing **Xbox 360 STFS package IDs** (Profile ID and/or Device ID). This is useful when you want to quickly reassign multiple savegames or content packages to a new profile or device.

The tool leverages **X360.dll** by *DJ Shepherd* for parsing and handling STFS files. I threw this together pretty quick so don't expect pretty code. It works and that's what matters.

---

## Usage

```bash
STFSIDChanger.exe <directory> <profileid | deviceid | both> <value1> [value2]
```

### Arguments

* **Arg 1**: Directory containing STFS packages (e.g. `savegames\`).
* **Arg 2**: What to change (`profileid`, `deviceid`, or `both`).
* **Arg 3**: New value (Profile ID if changing profileid, Device ID if changing deviceid, or Profile ID if changing both).
* **Arg 4**: *(only if Arg 2 is `both`)* Device ID value.

---

### Examples

Change only the **Profile ID** for all packages in a folder:

```bash
STFSIDChanger.exe "savegames\" profileid E000000000000000
```

Change only the **Device ID** for all packages in a folder:

```bash
STFSIDChanger.exe "savegames\" deviceid 0000000000000000000000000000000000000000
```

Change **both Profile ID and Device ID** at once:

```bash
STFSIDChanger.exe "savegames\" both E000000000000000 0000000000000000000000000000000000000000
```

---

## Credits

* **X360.dll** – created by [DJ Shepherd](https://github.com/djshepherd).

# Captain Capslock

Captain Capslock overrides the original behaviour of capslock and treats it as an additional modifier key, allowing applications to be easily switched with hotkeys.

E.g. to show Chrome, instead of alt-tabbing to it, simply press `Capslock + C` to bring it into focus. To hide it, press `Capslock + C` again.

## Features
- Configurable hotkeys to toggle applications.
- Option to launch on Windows startup.

## Non-Features
These features are not part of Captain Capslock, but may be in the future:
- Launching applications.
- UI configuration of hotkeys.

## Config
Application hotkeys and additional options are set in `Config.json`:
```json
{
  "Applications": [
    {
      "Process": "chrome",
      "Keys": [ "C" ]
    },
    {
      "Process": "explorer",
      "Keys": [ "E", "X" ]
    }
  ],
  "LaunchOnStartup": "True"
}
```

- `Applications`:
  - `Process`: Process name of the application to toggle.
  - `Keys`: List of hotkeys that toggle the application.
- `LaunchOnStartup`:
  - Set `True` to launch Captain Capslock when Windows startup.
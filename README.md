# Windows 10 Mobile To-Do App

A simple To-Do application built for Windows 10 Mobile using the Universal Windows Platform (UWP). It allows users to add and delete tasks with a responsive UI optimized for mobile devices.

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](https://opensource.org/licenses/MIT)

## Table of Contents
- [Features](#features)
- [Screenshots](#screenshots)
- [Requirements](#requirements)
- [Setup Instructions](#setup-instructions)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)

## Features

- **Add Tasks**: Create new tasks via a text input.
- **Delete Tasks**: Remove tasks with a single click.
- **Adaptive Design**: Responsive UI tailored for Windows 10 Mobile screens.

## Screenshots
| Surface Book | Emulator OS Starting | Emulator Main Screen | Emulator Task Added |
|-------------|--------------|--------------|--------------|
| ![Surface Book](Screenshots/surface_screenshot.png) | ![Emulator OS Starting](Screenshots/emulator_os_starting_screenshot.png) | ![Emulator Main Screen](Screenshots/emulator_main_screen_screenshot.png) | ![Emulator Task Added](Screenshots/emulator_task_added_screenshot.png) |

## Requirements

- Windows 10 Mobile (build 10.0.10240 or later).
- Visual Studio Community 2017 with the Universal Windows Platform development workload.
- Windows 10 SDK (version 10.0.10240 or later).

## Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/johnkoshy/Windows-10-Mobile-ToDo-App.git
2. Open the solution file (Win10-Mobile-ToDoApp.sln) in Visual Studio Community 2017.
3. Build the solution (Ctrl+Shift+B).
4. Enable Developer Mode on your Windows 10 Mobile device.
5. Select ARM for devices or x86 for emulators.
6. Deploy and run the app (F5).

## Usage

1. Launch the app on your Windows 10 Mobile device or emulator.
2. Enter a task in the text box and click Add Task.
3. Click Delete to remove a task from the list.

## Project Structure

- **ToDoPage.xaml**: Defines the UI (text input, add button, task list).
- **ToDoPage.xaml.cs**: Handles task addition and deletion logic.
- **Package.appxmanifest**: Configures the app’s display name and platform settings.
- **Assets/**: Contains app icons and tile images.
- **Screenshots/**: Stores app screenshots.

## Contributing

Contributions are welcome! Please submit a pull request or open an issue on GitHub for suggestions or bug reports.

## License
This project is licensed under [MIT License](LICENSE).
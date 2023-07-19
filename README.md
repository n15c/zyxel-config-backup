# ZyxelConfigBackup

## Description
The `ZyxelConfigBackup` is a utility for backing up the configurations of network devices. This utility is designed for .NET Windows Forms applications and uses Telnet for communication with devices.

The application allows the user to load a list of device IPs from a CSV file, then establishes a connection to each device, and executes a command to transfer the configuration to a TFTP server. The configurations are saved in a temporary path.

## Prerequisites

* A Windows environment with .NET Framework installed
* The necessary libraries (PrimS.Telnet)

## Usage

After launching the application, you'll see several options on the main form:

* **Read List**: Select a CSV file containing the IP addresses of the devices whose configurations you want to back up. After loading the file, the IP addresses will be displayed in the `Devices` list box and the number of devices will be displayed in the `Item Counter` label.
  
* **Clear List**: Clear the list of IP addresses from the `Devices` list box.

* **Start Download**: Initiate the backup process. The application will automatically detect the local IP of your machine, create a temporary directory for storing the backups, start a TFTP server, and initiate connections to each device to download the configurations. 

* **Username & Password**: Enter the necessary credentials to establish a Telnet connection with each device.

* **Log**: Displays a log of the application's activities. 

## Note

* Ensure that the devices you're connecting to support Telnet and are configured to allow connections from your machine.
* The TFTP server executable (`TFTPServer.exe`) must be located in the same directory as the application.
* The CSV file with the list of IP addresses should have each IP address separated by a semicolon (`;`).
* Ensure that you have the necessary permissions to execute tasks such as creating directories and running the TFTP server executable.

## Important

This script was created for educational purposes and should be used responsibly. Always ensure you have permission to connect to and download configurations from network devices. Misuse of this script could lead to violation of privacy and legal issues.

## Contributors
We appreciate your interest in our project. If you have any bugs or features you'd like to suggest, please submit an issue. We are always looking for improvements.

## License
The code in this project is licensed under MIT license.

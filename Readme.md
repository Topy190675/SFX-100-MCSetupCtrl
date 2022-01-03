# SFX-100 - Application control extension
This extension introduces a function using GUI (graphical user interface) to create new or edit / change existing configuration for SFB
Multi-Controller setup. This is nice because SimFeedback just provides GUI for its main SFX-100 controller. Available functions are :
- add new controller and set it up by defining controller's COM port (physical connection inside PC configuration; only available ports
  within your pc will be shown), user-defined controller-name.
- edit / change parameter for all already existing controllers (nice function if you e.g. have to change Arduino controller because of a
  defect since Windows most commonly will declare different Arduino with new COM port to avoid issue). Selection of controller to work on
  is done from drop-down list
- delete an existing controller configuration from active (useful if old, no longer needed controller has been removed)
FUTURE FUNCTION that will be implemented on next release
- this extension always creates a backup of your SimFeedback configuration (XML based) in advance of any change is done / written to your
  existing software config (previous XML config backups can be found within your SFB installation directory named "SimFeedback-bkp?.xml"
  where ? represents a number counting upwards with each save operation in advance of new created or edit / changed file will be saved) 
  

# Installation  
Do not click on the green Download button on this page. This would download the sources only.  
Download the files from the releases page instead or download latest version below 

https://github.com/topy190675/SFX-100-MulticontrollerSetup/releases

- Unzip / extract the archive into sub-folder of SFB's extension directory
- Enable the plugin in SFB and enable autorun of the extension in SimFeedback
- Restart SimFeedback and if everything works 

![image](https://github.com/topy190675/SFX-100-MulticontrollerSetup/blob/main/doc/ExtensionUsage.png?raw=true|width=200)


## How can someone be sure + test if this extension really is used and works as expected?
- Launch Simfeedback with debug logging enabled (SimFeedbackStart.exe -d 2)
- Check SimFeedbackLog.log in /log subdirectory 
- Look for entries containing "SFX-100 ProgCtrl"

This small extension is based on my research and concepts developed by SimFeedBack community (especially daCujo, Dsl71) 
and especially SFX-100 motion controller project's mastermind Saxxon.

**Please really support this fantastic project.**
https://opensfx.com


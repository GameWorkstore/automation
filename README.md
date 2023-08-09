# Automation
Automate your projects with this powerful tool with many build options!

# How to install

At package.json, add these 2 lines of code:
```json
"com.gameworkstore.automation": "https://github.com/GameWorkstore/automation.git#1.1.5"
"com.gameworkstore.patterns": "https://github.com/GameWorkstore/patterns.git#1.2.0"
```

And wait for unity to download and compile the package.

you can upgrade your version by including the release version at end of the link:
```json
"com.gameworkstore.automation": "https://github.com/GameWorkstore/automation.git#1.1.5"
```

# Automate Builds

On a windows .bat file, you invoke unity to build your game as the example below:
```bat
set CODEVERSION=23
set VERSION=1.0.%CODEVERSION%
call %UNITYPATH% -executeMethod GameWorkstore.Automation.BuildClass.BuildAndroid -projectPath %WORKSPACE% -gameversion %VERSION% -gamebundleversion %CODEVERSION%
```

You can also use Jenkins to attribute BUILD_NUMBER to CODEVERSION
```bat
set CODEVERSION=%BUILD_NUMBER%
```

or you can sum with a static number
```bat
set /a "CODEVERSION=%BUILD_NUMBER%+0"
```

# Arguments
## -builscript
name of the BuildScript asset.
The target buildscript of your game, like 'BuildScript.asset'.
don't forget to include '.asset' at end. 

## -gameversion
public version for the app/game. Use 1.0.0 for best results (applestore don't allow larger versions, some appstore are following).

## -gamebundleversion
exclusive code version for android and iOS. must be a integer.

# Build Methods

* BuildWindows
* BuildMacOS
* BuildLinux
* BuildAndroid
* BuildIOS
* BuildServerWindows
* BuildServerLinux
* BuildServerMacOS
* BuildWebGL
* BuildUWP

# Contributions

If you are using this library and want to submit a change, go ahead! Overall, this project accepts contributions if:
- Is a bug fix;
- Or, is a generalization of a well-known issue;
- Or is performance improvement;
- Or is an improvement to already supported feature.

Also, you can donate to allow us to drink coffee while we improve it for you!

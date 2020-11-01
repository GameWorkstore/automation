# Automation
Automate your projects with this powerful tool with many build options!

# How to install

At package.json, add these 2 lines of code:
> "com.gameworkstore.automation": "https://github.com/GameWorkstore/automation.git"

> "com.gameworkstore.patterns": "https://github.com/GameWorkstore/patterns.git"

And wait for unity to download and compile the package.

for update package for a newer version, click in [Help->PackageUpdate->GameWorkstore.Automation]

# Automate Builds

On a windows bat file, you invoke unity to build your game as the example below:
> set CODEVERSION=23

> set VERSION=1.0.0.%CODEVERSION%

> call %UNITYPATH% -executeMethod BuildClass.BuildAndroid -projectPath %WORKSPACE% -gameversion %VERSION% -bundleversion %CODEVERSION%

You can also use Jenkins to attribute BUILD_NUMBER to CODEVERSION

> set CODEVERSION=%BUILD_NUMBER%

or you can sum with a static number

> set /a "CODEVERSION=%BUILD_NUMBER%+0"

# Contributions

If you are using this library and want to submit a change, go ahead! Overall, this project accepts contributions if:
- Is a bug fix;
- Or, is a generalization of a well-known issue;
- Or is performance improvement;
- Or is an improvement to already supported feature.

Also, you can donate to allow us to drink coffee while we improve it for you!
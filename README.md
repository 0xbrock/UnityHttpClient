# Unity Http Client Experiment
This is an example project which loads a random [Flickr](https://www.flickr.com/) image display it in a scene.  It is an experiement to get a feel for how this would work in an actual project.  There are surely improvements that can be made to code structure, feel free to suggest improvements and build on it.  

## Setup

If Newtonsoft cannot be found errors exist in Visual Studio:
1. Open the FlickrLoader script in Visual Studio
2. In the Solution Explorer, expand the `UnityHttpClient` project and Refereces
3. Select `Analyzers`
4. In the Main Menu, click Project -> Add Reference...
5. Browse to the DLL in the plugins folder
6. Click OK

If the Newtonsoft cannot be found errors exist in Unity, but Visual Studio builds successfully:
1. In the Asset Explorer, Assets\Plugins, select `Newtonsoft.Json`
2. In the Inspector, `Select platforms for this plugin`, check `Any Platform` and uncheck the checkboxes in `Exclude Platforms`
3. Click Apply

That allowed everything to compile and run.

## Investigation Points
* Making GET requests
* Making POST requests
* Coroutines and yield returns structure/architecture 
* Load image and display image

## Contributing
1. Fork the repository
2. Create a feature branch: `git checkout -b awesome-feature`
3. Commit changes: `git commit -am 'A salient comment'`
4. Push the branch: `git push origin awesome-feature`
5. Create a pull request

## History
2017-07-06 Update gameObject names, Separate canvas/control to fix display sort layer issue

2017-06-20 Flickr GET calls, progress bar, UI enhancements

## Credits

## License
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

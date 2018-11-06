# EVR Label Manager
Third Party Add on for EmuVR to improve on the game scanning.

## about:
EVRLM takes the config from the EmuVR GameScanner and attempts to get more metadata, auto download cart/disc images and auto crop those to provide just the label, allowing you to to see the carts in game fully customized with very little effort.

## requirements:
* [EmuVR](http://www.emuvr.net/)
* [EmuMovies Account](https://emumovies.com/)

## usage:
* Open the app
* Enter the path to your EmuVR install in the first text box (eg. c:\emuVR\) - currently it MUST have a trailing slash.
* Enter your EmuMovies account details (be warned, currently the password is shown - AND STORED - in plain text)
* Click "Scan Games" to read the GameScanner playlist file and fetch the URLs of images from EmuMovies.
* Once Complete, Click "Download Carts" to download the cart images (to emuvrpath\Custom\Carts\) 
* Once that is complete, click "Convert Carts" to crop the cart images and put them in emuvrpath\Custom\Labels

## advanced usage:
In the application folder there is a system.cfg file. This can be edited with the in app properties window (bottom left) however this is currently not complete, so editing the cfg is recommended.

### System Names
The two most important properties configure EmuVRs media type and what EmuMovies knows that system as:

```javascript
  "EmuMoviesSystem": "Nintendo_NES",
  "EmuVRMedia": "NES"
```

The EmuMoviesSystem property is defined by emu movies - and is used for the cart image search. I have been discovering this with trial and error.

The EmuVRMedia property is what EmuVR calls this type. It can be found in the games subfolder/emuvr_core.txt, eg

```
media = "Nintendo 3DS"
core = "citra_libretro"
media_type = "Handheld"
```

would equate to 

```javascript
  "EmuVRMedia": "Nintendo 3DS"
```

### Image Size

EVRLM works on the principle that all cart images for a specific system will be the same aspect ratio with the same placement, but may be a different physical size. To make that easier to work with, we specify the size of the image you are working from and will use that as a ratio for any different sized images.

```javascript
    "ImageWidth": 444.0,
    "ImageHeight": 500.0,
```

### Label Position

The label position on the image is then configured with the LabelSize property, with the top left position and width/height stated:

```javascript
    "LabelSize": {
      "X": 176,
      "Y": 0,
      "Width": 214,
      "Height": 342
    }
```

### Templates

Some carts do not yet have proper cart models and thus their aspect ratio is incorrect. To help rectify this, misterobsidian on the EmuVR discord came up with a template (for NES carts) that you combine with the real label to generate a label in the correct aspect ratio that has the right 'feel'

To use these you must specify the image.

```javascript
  "Template": "Generic_Label.png",
```

and to configure it:

```javascript
    "TemplateLabelSize": {
      "X": 270,
      "Y": 0,
      "Width": 330,
      "Height": 450
    }
```

This specifies a rectangle to draw the label - it will be scaled to fit.

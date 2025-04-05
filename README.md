# GraphicAiFrontend
A dynamic graphic front end for local LLMs.

Supports the Windows platform.
Developed using Unity 6.
Tested with KoboldCPP running bartowski/Mistral-Nemo-Instruct-2407-GGUF.

Includes features:
* Ability to connect to Kobold-compatible APIs (KoboldCPP, KoboldAI, etc)
* Loading local images for the character and background display
* Customizable character name and context
* Customizable type and number of emotion categories for the character responses, which affects which graphics are used in display
* Speech to Text powered by whisper.cpp
* Long term memory: current dialog can be summarized and stored. This summary can be remembered and referenced by the bot even if the dialog is reset. 

![image](https://github.com/user-attachments/assets/d6e65799-fb06-4d0c-ba12-6d09380e3ac6)

# Installation

1. Download the .zip file from the latest release.
2. Extract and run the .exe to run the app.

# How to Use

![image](https://github.com/user-attachments/assets/055905e5-83d6-4189-b8db-cafd795ccf0a)

When loading into the app each time, a settings screen will appear. The settings here should be configured properly before continuing.
Steps that must be completed each time before you can continue are tagged with [COMPULSORY] here.
    
1. [COMPULSORY] URL: A Kobold-compatible URL in the format given. After entering, click the "connect" button to test the connection.
2. Emotions: Emotion categories for the character responses, which affects which graphics are used in display (discussed below)
3. [COMPULSORY] Character Folder: A folder containing the images to be used by the character, and where character data, chat logs are saved. After setting, click "Connect" to load images. Must contain following images:  
    1. body1.png
    2. body2.png
    3. body3.png
    4. body4.png
    5. background.png
    6. For each emotion, 3 images:   
        1. [emotion].png
        2. [emotion]_eyesclosed.png (used for blinking)
        3. [emotion]_mouthopen.png (used for talking)
4. Temperature and Repetition Penalty: Parameters to be passed to the LLM.
5. Prompt Formatting: How the prompt string is formatted before feeding into the model. Default values are based on bartowski/Mistral-Nemo-Instruct-2407-GGUF.

After completing all these steps, press "Save and Close" in the bottom right to start.  
Settings data is automatically saved and is loaded each time the app is opened.

# Controls

Most should be standard UI controls.

Aside from that, mouse click and drag the character portrait to move it. Mouse wheel to zoom.    
"Reset camera" button available near top left corner.

### Chat
Type the text you want to send into the text box at the bottom, then click "Send".

### Speech to Text
Press "STT" next to the text box to start recording, and press again to stop. The recording will be converted to text in the text box. (Powered by whisper.cpp)

### Long term memory
Use the "Summarize" feature in the top right corner to summarize the current dialog! The current dialog will be summarized and stored. This summary can be remembered and referenced by the bot even if the dialog is reset. (Turn "Using prev. summary" to off if not wanted)

# Saving Character Data

The character context, action dialog, chat logs are saved in the character folder that was set in the settings screen (same folder the images are in).  
Saving occurs automatically.



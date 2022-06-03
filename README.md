# Psychosis3DAvatar

## Introduction
This is the repository of psychosis pipeline for 3D avatar animation.
Due to the file size limit, part of the Agora plugin cannot be direclt pushed, if the project shows error when opening it, download the remaining plugin [here](https://drive.google.com/drive/folders/1kg8Ky-5PdxB_G8dJnf0l6RXQjDknCstI?usp=sharing) and place them under "Assets\AgoraEngine\Plugins"

## Some important features and plugins
- The Unity plugin to generate the avatar: [Avatar SDK](https://avatarsdk.com/)
> Process to generate a avatar suitable for LipSync
> 1. Create an account under Avatar SDK(remember to unsubscribe! Otherwise it will automatically charge you money!!!)
> 2. Select the bust option before uploading the image
> 3. Select the blendshape option and export as one model 

- The Unity plugin for lip synchronization: [SALSA LipSync](https://crazyminnowstudio.com/docs/salsa-lip-sync/modules/overview/)

## Backend
You can access the backend from:https://unicorn.cim.mcgill.ca/psychosis/tree/src 
The token is needed for first access after Unicorn’s every reboot
You can access the token by:
1. Run docker exec -it psychosis-stt-gpt2-tts /bin/bash to get inside the container 
2. Type jupyter notebook list to see the token

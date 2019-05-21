#!/usr/bin/env bash

if [ -z "$IOS_INSIGHTSKEY" ]
then
    echo "You need define the IOS_INSIGHTSKEY variable in App Center"
    exit
fi

if [ -z "$ANDROID_INSIGHTSKEY" ]
then
    echo "You need define the IOS_INSIGHTSKEY variable in App Center"
    exit
fi

#Set the IOS app insights key
./set-app-string-constant.sh ../src/Osma.Mobile.App/AppConstant.cs IosInsightKey $IOS_INSIGHTSKEY

#Set the Android app insights key
./set-app-string-constant.sh ../src/Osma.Mobile.App/AppConstant.cs AndroidInsightKey $ANDROID_INSIGHTSKEY

#!/usr/bin/env bash

if [ -z "$IOS_ANALYTICSKEY" ]
then
    echo "You need define the IOS_ANALYTICSKEY variable in App Center"
    exit
fi

if [ -z "$ANDROID_ANALYTICSKEY" ]
then
    echo "You need define the ANDROID_ANALYTICSKEY variable in App Center"
    exit
fi

#Set the IOS app insights key
./set-app-string-constant.sh ../src/Osma.Mobile.App/AppConstant.cs IosAnalyticsKey $IOS_ANALYTICSKEY

#Set the Android app insights key
./set-app-string-constant.sh ../src/Osma.Mobile.App/AppConstant.cs AndroidAnalyticsKey $ANDROID_ANALYTICSKEY

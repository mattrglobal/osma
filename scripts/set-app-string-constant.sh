#!/usr/bin/env bash
if [ "$#" -ne 3 ] 
then
    echo "Illegal number of parameters please specify APP_CONSTANT_FILE VARIABLE_NAME VARIABLE_VALUE in the order shown"
    exit
fi

APP_CONSTANT_FILE=$1
VARIABLE_NAME=$2
VARIABLE_VALUE=$3

if [ -e "$APP_CONSTANT_FILE" ]
then
    echo "Updating $VARIABLE_NAME to $VARIABLE_VALUE in $APP_CONSTANT_FILE"
    sed -i 's#'$VARIABLE_NAME' = "[-A-Za-z0-9:_./]*"#'$VARIABLE_NAME' = "'$VARIABLE_VALUE'"#' $APP_CONSTANT_FILE

    echo "File content:"
    cat $APP_CONSTANT_FILE
fi
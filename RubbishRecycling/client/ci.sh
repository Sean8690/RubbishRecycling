#!/bin/bash
set -e
PATH=$PATH:~/.local/bin # AWS CLI
PATH=$PATH:~/.dotnet/tools # dotnet global tools

# TO RUN:
# ./build.sh
# WITH env variables:
# - BUILD_NUMBER, set automatically by TeamCity, e.g. 1.0.21
# - octopus_api_key
# - OCTOPUS_PROJECT e.g. HomeScreen.Api
# - DOCKER_REGISTRY e.g. 087069701936.dkr.ecr.ap-southeast-2.amazonaws.com/home-screen-api

echo "Checking if all the required env variables have been set"
if [ -z $BUILD_NUMBER ] ;            then echo "ERROR: BUILD_NUMBER not found in env variables"            && exit 1; fi
if [ -z $octopus_api_key ] ;         then echo "ERROR: octopus_api_key not found in env variables"         && exit 1; fi
if [ -z $OCTOPUS_PROJECT ] ;         then echo "ERROR: OCTOPUS_PROJECT not found in env variables"         && exit 1; fi
if [ -z $DOCKER_REGISTRY ] ;         then echo "ERROR: DOCKER_REGISTRY not found in env variables"         && exit 1; fi
if [ -z $OCTUPUS_CHANNEL ] ;         then echo "ERROR: OCTUPUS_CHANNEL not found in env variables"         && exit 1; fi

echo "Starting build with number $BUILD_NUMBER"

echo "Logging into ECR"
$(aws ecr get-login --no-include-email --region ap-southeast-2)

echo "Building docker image $DOCKER_REGISTRY:$BUILD_NUMBER"
docker build -t $DOCKER_REGISTRY:$BUILD_NUMBER --build-arg BUILD_NUMBER=$BUILD_NUMBER .

echo "Pushing docker image to $DOCKER_REGISTRY:$BUILD_NUMBER"
docker push $DOCKER_REGISTRY:$BUILD_NUMBER

echo "Creating a new Octopus release into project $OCTOPUS_PROJECT"
dotnet-octo create-release --project=$OCTOPUS_PROJECT --server=http://ci.infotrack.com.au:88 --apiKey=$octopus_api_key --version=$BUILD_NUMBER --channel=${OCTUPUS_CHANNEL}

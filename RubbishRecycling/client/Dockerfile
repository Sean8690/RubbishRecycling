FROM node:14-alpine3.10 as build
ARG BUILD_NUMBER
WORKDIR /src
COPY ./.npmrc /src
COPY ./package.json /src
COPY ./package-lock.json /src
RUN npm i
COPY . /src
RUN npm run build -- --env.buildNr=${BUILD_NUMBER}
RUN cp ./dist/index.html ./server/index.html
RUN echo -n $BUILD_NUMBER > ./server/BUILD_NUMBER.txt

FROM amazon/aws-cli as uploader
ARG BUILD_NUMBER
WORKDIR /src
COPY --from=build /src/dist ./dist
RUN aws s3 sync ./dist s3://cf.infotrack.com.au/cdd/$BUILD_NUMBER --acl public-read

FROM node:14-alpine3.10 as runtime
COPY --from=build /src/server ./server
WORKDIR /server
RUN npm i
EXPOSE 80
ENTRYPOINT [ "node", "./server.js" ]

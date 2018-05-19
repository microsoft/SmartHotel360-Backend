FROM node:6-alpine
ENV NODE_ENV production
WORKDIR /usr/src/app
COPY ["package.json", "./"]
RUN npm install -g sequelize-cli
RUN npm install --production --silent && mv node_modules ../
COPY . .
EXPOSE 80
CMD npm start
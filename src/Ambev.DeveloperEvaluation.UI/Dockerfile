# Stage 1 - Build Angular
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci --no-fund --no-audit || npm install --no-fund --no-audit
COPY . .
RUN npm run build

# Stage 2 - Nginx (serve estático + injeta env.js em runtime)
FROM nginx:alpine
COPY ops/nginx/default.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/Ambev.DeveloperEvaluation.UI/ /usr/share/nginx/html/
COPY ops/nginx/docker-entrypoint.sh /docker-entrypoint.d/50-env.sh
RUN chmod +x /docker-entrypoint.d/50-env.sh
EXPOSE 80

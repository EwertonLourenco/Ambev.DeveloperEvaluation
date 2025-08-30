# Stage 1 - Build Angular
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci --no-fund --no-audit || npm install --no-fund --no-audit
COPY . .
RUN npm run build

# Stage 2 - Nginx
FROM nginx:alpine

# (1) limpa a pasta padrão do nginx
RUN rm -rf /usr/share/nginx/html/*

# (2) copia apenas o CONTEÚDO de .../browser para a raiz
#     (repare na barra final em 'browser/')
COPY --from=build /app/dist/*/browser/ /usr/share/nginx/html/

# (3) seu conf + script de env
COPY ops/nginx/default.conf /etc/nginx/conf.d/default.conf
COPY ops/nginx/docker-entrypoint.sh /docker-entrypoint.d/50-env.sh
RUN sed -i 's/\r$//' /docker-entrypoint.d/50-env.sh && chmod +x /docker-entrypoint.d/50-env.sh

EXPOSE 80

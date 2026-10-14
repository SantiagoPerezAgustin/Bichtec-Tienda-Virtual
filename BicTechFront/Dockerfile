# Etapa de compilación
FROM node:20-alpine AS build
WORKDIR /app

# Argumento para la URL de la API (para que sepa a dónde apuntar el frontend)
ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

# Etapa de producción con Nginx
FROM nginx:alpine AS final

# Configuración de Nginx para manejar rutas de React (SPA)
RUN echo 'server { \
    listen 80; \
    location / { \
        root /usr/share/nginx/html; \
        index index.html index.htm; \
        try_files $uri $uri/ /index.html; \
    } \
}' > /etc/nginx/conf.d/default.conf

COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

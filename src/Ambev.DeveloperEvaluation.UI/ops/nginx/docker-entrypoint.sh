#!/bin/sh
set -e

: "${API_BASE_URL:=http://ambev.developerevaluation.webapi:8080}"

cat >/usr/share/nginx/html/env.js <<EOF
window.__env = { API_BASE_URL: "${API_BASE_URL}" };
EOF

echo "[env] API_BASE_URL=${API_BASE_URL}"

import path from "path";
import vue from "@vitejs/plugin-vue";
import { defineConfig } from "vite";

const defaultConfig = {
  plugins: [vue()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src")
    }
  }
};

// https://vitejs.dev/config/
export default defineConfig(({ command }) => {
  if (command === "serve") {
    return {
      ...defaultConfig,
      server: {
        port: 8080,
        proxy: {
          "^/api": {
            target: "http://localhost:5231",
            changeOrigin: true
          }
        }
      }
    };
  }
  else {
    return defaultConfig;
  }
});

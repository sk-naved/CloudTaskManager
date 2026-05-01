import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
    plugins: [react()],
    server: {
        port: 5173,
        proxy: {
            // Any request starting with /api will be sent to the C# server
            '/api': {
                target: 'http://localhost:5556', // <--- Change this to 5556
                changeOrigin: true,
                secure: false,
            }
        }
    }
})
const CACHE_NAME = 'kyk-yemek-v1';
const ASSETS_TO_CACHE = [
  '/',
  '/css/styles.css',
  '/js/site.js',
  '/images/icon-512.png',
  '/images/Backgroundss.webp'
];

// Service Worker Kurulumu
self.addEventListener('install', event => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => cache.addAll(ASSETS_TO_CACHE))
  );
});

// Service Worker Aktivasyonu
self.addEventListener('activate', event => {
  event.waitUntil(
    caches.keys().then(cacheNames => {
      return Promise.all(
        cacheNames.map(cache => {
          if (cache !== CACHE_NAME) {
            return caches.delete(cache);
          }
        })
      );
    })
  );
});

// İstekleri Yakalama (Fetch)
self.addEventListener('fetch', event => {
  event.respondWith(
    caches.match(event.request)
      .then(response => {
        // Cache'te varsa döndür, yoksa ağdan çek
        return response || fetch(event.request);
      })
  );
});

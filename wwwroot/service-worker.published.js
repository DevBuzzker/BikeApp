// Incrementing VERSION forces all clients to reload the SW
const VERSION = '1.0.0';
const CACHE   = `ebike-cache-${VERSION}`;

self.addEventListener('install', e =>
  e.waitUntil(
    caches.open(CACHE).then(c =>
      c.addAll(self.assetsManifest.assets.map(a => a.url))
    )
  )
);

self.addEventListener('activate', e =>
  e.waitUntil(
    caches.keys().then(keys =>
      Promise.all(keys.filter(k => k !== CACHE).map(k => caches.delete(k)))
    )
  )
);

self.addEventListener('fetch', e => {
  if (e.request.method !== 'GET') return;
  e.respondWith(
    caches.match(e.request).then(cached =>
      cached ?? fetch(e.request).then(resp => {
        const clone = resp.clone();
        caches.open(CACHE).then(c => c.put(e.request, clone));
        return resp;
      })
    )
  );
});

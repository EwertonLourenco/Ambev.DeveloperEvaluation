import { HttpInterceptorFn } from '@angular/common/http';

export const BaseUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const base = (window as any).__env?.API_BASE_URL ?? 'http://localhost:8080';

  // já absoluta? segue
  if (/^https?:\/\//i.test(req.url)) {
    console.debug('[HTTP passthrough]', req.method, req.url);
    return next(req);
  }

  const url = base.replace(/\/$/, '') + '/' + req.url.replace(/^\//, '');
  console.debug('[HTTP]', req.method, url, { headers: req.headers.keys() });
  return next(req.clone({ url }));
};

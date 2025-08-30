import { HttpInterceptorFn } from '@angular/common/http';
import { API_BASE_URL } from '../env';

export const BaseUrlInterceptor: HttpInterceptorFn = (req, next) => {
  if (/^https?:\/\//i.test(req.url)) {
    return next(req); // já é absoluta
  }
  const url = API_BASE_URL.replace(/\/$/, '') + '/' + req.url.replace(/^\//, '');
  return next(req.clone({ url }));
};

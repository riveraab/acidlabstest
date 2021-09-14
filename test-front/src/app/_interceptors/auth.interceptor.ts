import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/internal/Observable";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    intercept(req: HttpRequest<any>,
              next: HttpHandler): Observable<HttpEvent<any>> {

        const token = sessionStorage.getItem("token");

        if (token) {
            
            const cloned = req.clone({
                headers: req.headers.set("Authorization",
                    "Bearer " + JSON.parse(token))
            });

            return next.handle(cloned);
        }
        else {
            return next.handle(req);
        }
    }
}
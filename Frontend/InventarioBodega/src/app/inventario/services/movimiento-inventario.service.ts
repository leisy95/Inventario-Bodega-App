import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class MovimientoInventarioService {
    private apiUrl = 'https://localhost:5001/api/MovimientoInventario';

    constructor(private http: HttpClient) { }

    getMovimientosByItem(idItem: number): Observable<any[]> {
        return this.http.get<any[]>(`${this.apiUrl}/item/${idItem}`);
    }

    getMovimientosByReferencia(referencia: string): Observable<any[]> {
        return this.http.get<any[]>(`${this.apiUrl}/referencia/${referencia}`);
    }
}
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// Modelo que coincide con tu clase Inventario.cs

export interface Inventario {
    id: number;
    referencia: string;
    descripcion: string;
    tipoDeBolsa: string;
    tipoMaterial: string;
    densidad: string;
    color: string;
    segundoColor: string;
    impresoNo: string;
    ancho: number;
    alto: number;
    calibre: number;
    peso: number;

    // Extras opcionales para la vista
    fl?: number;
    ff?: number;
    fs?: number;
    unidad?: string;
    troquel?: string;
    referenciaImpresa?: string;
    paqueteNumero?: number;
    paqueteCantidad?: number;
}
@Injectable({
    providedIn: 'root'
})
export class InventarioService {
    private apiUrl = 'http://localhost:5244/api/inventario'; // URL de tu backend

    constructor(private http: HttpClient) { }

    // Obtener todos los registros
    getInventarios(): Observable<Inventario[]> {
        return this.http.get<Inventario[]>(this.apiUrl);
    }

    // Obtener un inventario por id
    getInventario(id: number): Observable<Inventario> {
        return this.http.get<Inventario>(`${this.apiUrl}/${id}`);
    }

    //Obtener una refrencia por refrencia 
    getByReferencia(referencia: string): Observable<Inventario> {
        return this.http.get<Inventario>(`${this.apiUrl}/buscar/${referencia}`);
    }

    // Método para ingresar peso y acumular cantidad
    ingresarPeso(referencia: string, peso: number): Observable<any> {
        return this.http.post<any>(`${this.apiUrl}/ingresar-peso`, { referencia, peso });
    }

    // Crear un nuevo inventario
    addInventario(item: Inventario): Observable<Inventario> {
        return this.http.post<Inventario>(this.apiUrl, item);
    }

    // Actualizar un inventario
    updateInventario(id: number, item: Inventario): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/${id}`, item);
    }

    // Eliminar un inventario
    deleteInventario(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
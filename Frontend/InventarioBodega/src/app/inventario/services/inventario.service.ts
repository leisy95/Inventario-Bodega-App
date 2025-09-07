import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// Modelo que coincide con tu clase Inventario.cs

export interface Inventario {
    id: number;
    referencia: string;
    descripcion: string;
    tipoBolsa: string;
    tipoMaterial: string;
    densidad: string;
    color: string;
    segundoColor: string;
    impresoNo: string;
    ancho: number;
    alto: number;
    calibre: number;
    peso: number;
    //AQUI EDITE
    cantidad?: number;
    referenciaNormalizada?: string;

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

export interface RegistrarItemRequest {
    Referencia: string;
    Descripcion: string;
    TipoBolsa?: string;
    TipoMaterial?: string;
    ImpresoNo: string;
    Ancho?: number;
    Alto?: number;
    Calibre?: number;
    Color: string;
    SegundoColor?: string;
    Densidad?: string;
}

@Injectable({
    providedIn: 'root'
})
export class InventarioService {
    private apiUrl = 'http://localhost:5244/api/inventario';

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
    addInventario(payload: RegistrarItemRequest): Observable<any> {
        return this.http.post<any>(`${this.apiUrl}/registrar-item`, payload);
    }

    // eliminar un inventario
    deleteInventario(id: number) {
        return this.http.delete(`${this.apiUrl}/${id}`);
    }

    //Actualizar un inentario
    updateInventario(id: number, inventario: any) {
        return this.http.put(`${this.apiUrl}/${id}`, inventario);
    }
}
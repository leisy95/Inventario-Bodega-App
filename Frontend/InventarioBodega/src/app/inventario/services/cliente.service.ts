import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CrearCliente } from '../models/cliente.model';

export interface Cliente {
    id?: number;
    nombre: string;
    direccion?: string;
    telefono?: string;
    email?: string;
}

@Injectable({
    providedIn: 'root'
})
export class ClienteService {
    private apiUrl = 'http://localhost:5244/api/clientes';

    constructor(private http: HttpClient) { }

    // Crear cliente
    crearCliente(cliente: Cliente): Observable<any> {
        return this.http.post<any>(`${this.apiUrl}/crear`, cliente, {
            headers: { 'Content-Type': 'application/json' }
        });
    }

    // Listar clientes
    listarClientes(): Observable<Cliente[]> {
        return this.http.get<Cliente[]>(`${this.apiUrl}/listar`);
    }

    // Obtener cliente por id
    obtenerCliente(id: number): Observable<Cliente> {
        return this.http.get<Cliente>(`${this.apiUrl}/${id}`);
    }
}
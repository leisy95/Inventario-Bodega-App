import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BuscarSalidasComponent } from './buscar-salidas.component';

describe('BuscarSalidasComponent', () => {
  let component: BuscarSalidasComponent;
  let fixture: ComponentFixture<BuscarSalidasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BuscarSalidasComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BuscarSalidasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

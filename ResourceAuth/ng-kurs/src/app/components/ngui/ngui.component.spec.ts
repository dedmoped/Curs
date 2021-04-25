import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NguiComponent } from './ngui.component';

describe('NguiComponent', () => {
  let component: NguiComponent;
  let fixture: ComponentFixture<NguiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NguiComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NguiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

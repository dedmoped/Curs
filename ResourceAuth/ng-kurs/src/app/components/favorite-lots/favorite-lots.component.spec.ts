import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavoriteLotsComponent } from './favorite-lots.component';

describe('FavoriteLotsComponent', () => {
  let component: FavoriteLotsComponent;
  let fixture: ComponentFixture<FavoriteLotsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FavoriteLotsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FavoriteLotsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

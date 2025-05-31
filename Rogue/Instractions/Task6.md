# Etap 6 Zmiana zachowania przeciwników

## Cel zadania

Implementacja logiki różnych zachowań przeciwników.


## Wymagania

### Zmiana zachowania przeciwników

Celem zadania jest rozszerzenie logiki przeciwników o ich zachowanie. Wpływa ono na to, w jaki sposób przeciwnik porusza się po labiryncie, oraz czy atakuje gracza.

Przykładowe zachowania:
- przeciwnik spokojny: Nie porusza się po labiryncie, nie atakuje niesprowokowany.
- przeciwnik agresywny: Jeżeli w jego pobliżu znajdzie się gracz, porusza się w jego kierunku, i atakuje go jeżeli jest w zasięgu.
- przeciwnik płochliwy: Kiedy w jego pobliżu znajdzie się gracz, stara się od niego uciec, poruszając się z dala od jego aktualnej pozycji.

Dodawanie nowych zachowań, jak i zmiana zachowania przeciwnika podczas działania programu powinna być łatwa(np. kiedy zdrowie przeciwnika, osiągnie niski poziom, może on zacząć uciekać przed graczem)


Każdy przeciwnik powinien móc okazywać każdy rodzaj zachowania.

Do implementacji tego zadania, należy wykorzystać wzorzec Strategii
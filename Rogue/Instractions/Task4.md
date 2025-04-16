Etap 4: Walka z wrogami

## Cel zadania
Stworzenie mechanizmu umożliwiającego walkę z wrogami obecnymi na planszy.

## Wymagania
- walka z wrogami

### UWAGA!!!
Całość należy zaimplementować korzystając ze wzorca visitor.
Rzutowanie nie jest zakazane, ale nie może zostać wykorzystywane zamiast visitora. Rzutować można do typów ogólniejszych lub w momentach gdy dokładnie znamy typ obiektu.
Rozwiązania korzystające z pól identyfikujących typ obiektu (enumy, nazwy klas, itp.), nie będą akceptowane.
Nie chcemy rozbudowywać klas broni o dodatkowe metody.
Powinno łatwo być rozwinąć grę o dodatkowe metody ataku/obrony oraz nowe rodzaje broni.
Rozwiązanie powinno nie wykorzystywać wielu instrukcji if ani switch.

### Walka z wrogami

W labiryncie znajdują się przeciwnicy, z którymi gracz może walczyć. Podczas walki gracz wykonuje wybrany atak bronią trzymaną w ręku lub w obu rękach lub dwoma broniami w dwóch rękach, a następnie zostaje zaatakowany przez przeciwnika. 

Każdy przeciwnik ma następujące statystyki:
- punkty życia,
- wartość ataku,
- punkty pancerza.

Obrażenia otrzymywane przez przeciwnika są pomniejszane o wartość jego pancerza. Dla uproszczenia wartość ataku danego przeciwnika jest zawsze stała ale może być też częściowo losowa w różnych wariantach gry.
Obrażenia otrzymywane przez gracza zależą od wartości ataku przeciwnika oraz siły obrony gracza. 
Wartość obrony gracza wynika z własnych umiejętności oraz wartości defensywnych broni, które trzyma w ręku.  

Jeśli punkty życia przeciwnika zostaną zredukowane do 0 zostaje on usunięty z planszy.
Jeśli punkty życia gracza zostaną zredukowane do 0 przegrywa on grę. Powinna zostać wtedy wyświetlona stosowna informacja.

Każda z broni wykorzystywanych przez gracza powinna należeć do jednej z 3 kategorii:
- broń ciężka (obrażenia zależą od siły i agresji),
- broń lekka (obrażenia zależą od zręczności i szczęścia),
- broń magiczna (obrażenia zależą od mądrości), 

Podczas walki gracz ma do wyboru 3 następujące ataki (niezależnie od trzymanej broni):
- atak zwykły,
- atak skryty,
- atak magiczny.
  
W przypadku ataku zwykłego obrażenia dla broni ciężkiej i lekkiej pozostają bez zmian, a dla magicznej wynoszą 1.
W przypadku ataku skrytego obrażenie dla broni lekkiej są podwojone, dla ciężkiej zredukowane do połowy, a dla magicznej wynoszą 1.
W przypadku ataku magicznego obrażenia dla broni magicznej pozostają bez zmian, a dla pozostałych rodzajów broni wynoszą 1
Jeśli gracz ma w rękach przedmiot nie będący bronią obrażenia zawsze wynoszą 0.

Siła obrony gracza zależy od zastosowanego ataku i wyposażonej broni.
W przypadku zwykłego ataku obrona to:
- dla broni ciężkiej: siła + szczęście,
- dla broni lekkiej: zręczność + szczęście,
- dla broni magicznej: zręczność + szczęśćie.
- dla pozostałych przedmiotów: zręczność
  
W przypadku skrytego ataku:
- dla broni ciężkiej: siła,
- dla broni lekkiej: zręczność,
- dla broni magicznej: 0
- dla pozostałych przedmiotów: 0
  
W przypadku ataku magicznego:
- dla broni ciężkiej: szczęście,
- dla broni lekkiej: szczęście,
- dla broni magicznej: mądrość * 2
- dla pozostałych przedmiotów: szczęście
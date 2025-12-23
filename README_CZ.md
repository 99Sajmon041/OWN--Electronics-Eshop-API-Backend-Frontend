# Electronics E-Shop

## Popis projektu
Tento projekt představuje plnohodnotnou e-shopovou aplikaci zaměřenou na backendovou architekturu a reálné obchodní scénáře. Cílem bylo navrhnout přehledné, rozšiřitelné a dlouhodobě udržovatelné řešení s důrazem na čistou architekturu a správné oddělení odpovědností.

Aplikace umožňuje zákazníkům procházet produkty, pracovat s nákupním košíkem, vytvářet objednávky a spravovat svůj účet. Administrátorská část poskytuje správu objednávek, změnu jejich stavů a přehled nad celým procesem objednávek.

---

## Hlavní funkce
- Registrace, přihlášení a správa uživatelského účtu
- Nákupní košík (přidávání položek, úprava množství, odebrání)
- Vytváření a zpracování objednávek
- Stavový životní cyklus objednávky (nová, zaplacená, odeslaná, dokončená, zrušená)
- Administrátorská správa objednávek
- Odesílání e-mailů (např. kontakt na podporu)
- Validace vstupů a ošetření chybových stavů

---

## Použité technologie a architektura
- ASP.NET Core
- Clean Architecture
- CQRS
- MediatR
- Entity Framework Core
- Repository a Unit of Work pattern
- ASP.NET Identity (autentizace a autorizace)
- REST API
- Logging a vlastní aplikační výjimky

---

## Architektonický přehled
Projekt je rozdělen do logických vrstev:
- **Domain** – entity, enumy a rozhraní
- **Application** – aplikační logika, příkazy, handlery a validace
- **Infrastructure** – databázová vrstva, repository a externí služby
- **API / Web** – rozhraní aplikace

Toto rozdělení umožňuje snadné testování, rozšiřování a dlouhodobou údržbu projektu.

---

## Stav projektu
Projekt je dokončen. Funkcionalita byla implementována, otestována a slouží jako referenční ukázka backendového e-shopového řešení.

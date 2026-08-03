# Decisiones técnicas

## 1. Decisión técnica

Utilice una arquitectura monolitica separada por capas para el backend.

Descarte cualquier otro tipo de arquitectura.

Lo hice porque he trabajado de esta manera y entiendo la responsabilidad de cada capa y que es lo que representan en el sistema.

---

## 2. Decisión técnica

Crear una capa Domain donde se representa la maquina de estados, el calculo del SLA y los permisos por rol. 

Descarte crear un servicio para cada una de estas funcionalidades.

Lo hice de esta manera porque asi puedo centralizar las clases y metodos que desarrollan la logica de la maquina de estados, el SLA y los permisos para luego poder implementarlos en los servicios.

---

## 3. Decisión técnica

Centralizar el manejo de errores.

Repetir codigo en los services o controllers.

Lo hice de esta manera para reducir codigo y entregar algo mas limpio, adicionalmente de esta manera se cumple con el formato de error solicitado en la prueba

---

## Uso de IA

Para esta prueba la IA fue de mucha ayuda para escribir el codigo ya que es la primera vez que trabajo con C#
pero esto no quiere decir que no conozca los conceptos y la logica de programacion que use, se cuales son las responsabilidades de cada capa que se implemento como los models, dtos, controllers o services. El frontend es una parte que puedo decir que no es mi fuerte pero tambien entiendo las responsabilidades de las capas.

---

## Si tuviera una semana más

Si tuviera una semana mas desarrollaria toda la parte del frontend para dejarla al 100% luego veria el comportamiento del sistema para verficar que no tenga ninguna irregularidad.

---

## Principal dificultad

Problema al capturar el id del usuario que se logeaba.

Con ayuda de la IA identifique la causa del porque el id no se capturaba luego de eso cambie la forma en la que se capturaba la informacion del usuario por medio del token que generaba el servidor.
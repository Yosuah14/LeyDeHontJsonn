using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace LeyDeHont.Domain
{
    internal class PartiesDataComponent
    {
        // Ruta del archivo JSON donde se almacenan los datos de los partidos
        public static string Path = "C:\\Users\\Alumno\\Desktop\\LeyDeHontJsonn2\\Data\\Partidos.json";

        // Método para leer la lista de partidos desde el archivo JSON
        public static ObservableCollection<DatosPartido> ReadParties()
        {
            string contenidoJson = File.ReadAllText(Path);

            RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(contenidoJson);

            return rootObject?.Parties ?? new ObservableCollection<DatosPartido>();
        }
        // Método para insertar un nuevo partido en la lista y actualizar el archivo JSON
        public static void InsertParty(DatosPartido newParty)
        {
            // Obtiene la lista actual de partidos
            ObservableCollection<DatosPartido> parties = ReadParties();

            // Agrega el nuevo partido a la lista
            parties.Add(newParty);

            // Crea un nuevo objeto RootObject con la lista actualizada
            RootObject rootObject = new RootObject
            {
                Parties = parties
            };

            // Serializa y guarda el objeto en el archivo JSON
            if (rootObject != null)
            {
                string contenidoJson = JsonConvert.SerializeObject(rootObject, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(Path, contenidoJson);
            }
        }

        // Método para actualizar o insertar un partido en la lista y actualizar el archivo JSON
        public static void UpdateOrInsertParty(DatosPartido updatedParty)
        {
            // Obtiene la lista actual de partidos
            ObservableCollection<DatosPartido> parties = ReadParties();

            // Busca el partido que coincide con el nombre del partido actualizado
            DatosPartido existingParty = parties.FirstOrDefault(p => p.Nombre.Equals(updatedParty.Nombre));

            if (existingParty != null)
            {
                // Si existe un partido con el mismo nombre, lo actualiza
                parties.Remove(existingParty);
            }

            // Agrega el partido actualizado a la lista
            parties.Add(updatedParty);

            // Crea un nuevo objeto RootObject con la lista actualizada
            RootObject rootObject = new RootObject
            {
                Parties = parties
            };

            // Serializa y guarda el objeto en el archivo JSON
            string contenidoJson = JsonConvert.SerializeObject(rootObject, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(Path, contenidoJson);
        }

        public static void DeleteParty(DatosPartido partyToDelete)
        {
            // Obtiene la lista actual de partidos
            ObservableCollection<DatosPartido> parties = ReadParties();

            // Busca el partido que coincide con el nombre del partido proporcionado
            DatosPartido partyToRemove = parties.FirstOrDefault(p => p.Nombre.Equals(partyToDelete.Nombre));

            if (partyToRemove != null)
            {
                // Si se encuentra el partido, lo elimina de la lista
                parties.Remove(partyToRemove);

                // Crea un nuevo objeto RootObject con la lista actualizada
                RootObject rootObject = new RootObject
                {
                    Parties = parties
                };

                // Serializa y guarda el objeto en el archivo JSON
                string contenidoJson = JsonConvert.SerializeObject(rootObject, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(Path, contenidoJson);
            }
        }
        public static void UpdateAllPartiesInJson(ObservableCollection<DatosPartido> parties)
        {
            // Obtener la lista actual de partidos desde el archivo JSON

            foreach (DatosPartido partido in parties)
            {
                // Encontrar el partido correspondiente en la lista
                DatosPartido existingParty = parties.FirstOrDefault(p => p.Nombre.Equals(partido.Nombre));

                if (existingParty != null)
                {
                    // Actualizar los datos del partido existente con los nuevos valores
                    existingParty.Nombre = partido.Nombre;
                    existingParty.Acronimo = partido.Acronimo;
                    existingParty.Presidente = partido.Presidente;
                    existingParty.Seats = partido.Seats;
                    existingParty.Votes = partido.Votes;
                }
            }

            // Crear un nuevo objeto RootObject con la lista actualizada
            RootObject rootObject = new RootObject
            {
                Parties = parties
            };

            // Serializar y guardar el objeto en el archivo JSON
            string contenidoJson = JsonConvert.SerializeObject(rootObject, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(Path, contenidoJson);
        }
        // Clase que representa el objeto raíz para la serialización JSON
        class RootObject
        {
            [JsonProperty("partidos")]
            public ObservableCollection<DatosPartido> Parties { get; set; }
        }
    }
}


﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TECNM.Residencias.Data.Migrations.Properties {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TECNM.Residencias.Data.Migrations.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a CREATE TABLE loc_country (
        ///    id   INTEGER PRIMARY KEY
        ///                 NOT NULL,
        ///    name TEXT    UNIQUE
        ///                 NOT NULL
        ///)
        ///STRICT;
        ///
        ///CREATE TABLE loc_state (
        ///    id         INTEGER PRIMARY KEY
        ///                       NOT NULL,
        ///    country_id INTEGER REFERENCES loc_country (id) ON DELETE RESTRICT
        ///                       NOT NULL,
        ///    name       TEXT    NOT NULL,
        ///    CONSTRAINT ak_loc_state_identity UNIQUE (
        ///        country_id,
        ///        name
        ///    )
        ///)
        ///STRICT;
        ///
        ///CREATE TABLE loc_city  [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string migration_1 {
            get {
                return ResourceManager.GetString("migration_1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a CREATE TRIGGER tgg_itcm_company_insert
        ///AFTER INSERT ON itcm_company
        ///BEGIN
        ///    INSERT INTO itcm_company_search (rowid, rfc, name)
        ///    VALUES (NEW.id, NEW.rfc, NEW.name);
        ///END;
        ///
        ///CREATE TRIGGER tgg_itcm_company_update
        ///AFTER UPDATE ON itcm_company
        ///WHEN NEW.rfc != OLD.rfc OR NEW.name != OLD.name
        ///BEGIN
        ///    UPDATE itcm_company_search
        ///    SET rfc = NEW.rfc, name = NEW.name
        ///    WHERE rowid = NEW.id;
        ///END;
        ///
        ///CREATE TRIGGER tgg_itcm_company_delete
        ///AFTER DELETE ON itcm_company
        ///BEGIN
        ///    DELETE FROM itcm [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string migration_2 {
            get {
                return ResourceManager.GetString("migration_2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a INSERT INTO loc_country VALUES (1, &apos;México&apos;);
        ///INSERT INTO loc_state VALUES (1, 1, &apos;Aguascalientes&apos;);
        ///INSERT INTO loc_state VALUES (2, 1, &apos;Baja California&apos;);
        ///INSERT INTO loc_state VALUES (3, 1, &apos;Baja California Sur&apos;);
        ///INSERT INTO loc_state VALUES (4, 1, &apos;Campeche&apos;);
        ///INSERT INTO loc_state VALUES (5, 1, &apos;Chiapas&apos;);
        ///INSERT INTO loc_state VALUES (6, 1, &apos;Chihuahua&apos;);
        ///INSERT INTO loc_state VALUES (7, 1, &apos;Ciudad de México&apos;);
        ///INSERT INTO loc_state VALUES (8, 1, &apos;Coahuila&apos;);
        ///INSERT INTO loc_state VALUES (9, 1, &apos;Colim [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string migration_3 {
            get {
                return ResourceManager.GetString("migration_3", resourceCulture);
            }
        }
    }
}

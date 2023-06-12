# Миграции

Для создания миграций используется библиотека [FluentMigrator 3.3.2](https://fluentmigrator.github.io). Она позволяет создавать обновления для базы данных в удобном и читаемом формате без обязанности знания SQL синтаксиса.

## Утилиты

Для начала, давайте установим вспомогательные утилиты, выполнив команду из Nuget в вашем терминале:

- [AddMigration](https://www.nuget.org/packages/Tenogy.Tools.FluentMigrator.AddMigration) – Создание миграций
- [UpdateDatabase](https://www.nuget.org/packages/Tenogy.Tools.FluentMigrator.UpdateDatabase) – Применение миграций.

После установки утилит в терминале вашей IDE будут доступны две новые команды:

```shell
add-migration --help
update-database --help
```
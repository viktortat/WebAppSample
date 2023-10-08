## Работа с Git flow

git flow init -d

- [Шпаргалка по git flow](https://danielkummer.github.io/git-flow-cheatsheet/index.ru_RU.html)

      git flow feature start MYFEATURE
      git flow feature finish MYFEATURE
      git flow feature publish MYFEATURE

## Разное

```
git add .
git commit -m "Initial commit"

git push -u origin master
git remote rename origin old-origin
git remote add origin git@gitlab.com:viktortat/myappcore.git
git push -u origin --all
git push -u origin --tags
git config --global user.name "Виктор Тат"
git config --global user.email "viktortat@gmail.com"

git config --global core.autocrlf true
git config --global core.safecrlf warn

touch README.md
```

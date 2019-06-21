pipeline {
    agent any

    options {
        disableConcurrentBuilds()
        buildDiscarder( logRotator( numToKeepStr: '15') )
    }

    environment {
        testImage = "service-identification-tests"
    }

    stages {
        stage("Build") {
            parallel {
                stage('Tests') {
                    steps {
                        sh "docker build --no-cache -f tests/Dockerfile -t ${testImage} ."
                    }
                }
            }
        }

        stage('Unit tests') {
            steps {
                sh "docker container run --rm ${testImage}"
            }
        }

        stage('Cleanup') {
            parallel {
                stage('Tests') {
                    steps {
                        script {
                            sh "docker rmi --force `docker images -q ${testImage} | uniq`"
                        }
                    }
                }
            }
        }
    }
}
